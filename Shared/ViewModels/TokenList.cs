using ParseTreeVisualizer.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ParseTreeVisualizer.ViewModels {
    // Changes to IsSelected are tracked for elements of the TokenList.
    // When a change is detected, the collection's properties are recalculated.
    // This is done via the underlying field, and not through the property setter, so IsSelected of child elements is not affected

    // When the collection's properties are set, the selection also needs to change
    // Change notifications on IsSelected to the collection are ignored, while within the call to the property setter

    [Serializable]
    public class TokenList : ObservableCollection<Token>, INotifyPropertyChanged {
        private readonly PropertyChangedEventHandler tokenSelectionHandler;

        public TokenList() {
            tokenSelectionHandler = (s, e) => {
                if (e.PropertyName == nameof(Token.IsSelected)) {
                    if (inUpdateSelection) { return; }
                    recalculateProperties();
                }
            };

            CollectionChanged += (s, e) => {
                if (e.OldItems != null) {
                    foreach (var token in e.OldItems.Cast<Token>()) {
                        token.PropertyChanged -= tokenSelectionHandler;
                    }
                }

                if (e.NewItems != null) {
                    foreach (var token in e.NewItems.Cast<Token>()) {
                        token.PropertyChanged += tokenSelectionHandler;
                    }
                }

                recalculateProperties();
            };
        }

        private bool inUpdateSelection;
        private void updateSelection(int? startChar, int? endChar) {
            inUpdateSelection = true;
            foreach (var token in this) {
                if (startChar != null && endChar != null) {
                    token.IsSelected = token.Span.start <= endChar && token.Span.stop >= startChar;
                } else {
                    token.IsSelected = false;
                }
            }
            inUpdateSelection = false;
        }

        private void recalculateProperties() {
            int? newStartChar = null;
            int? newEndChar = null;
            int? newMaxTokenTypeID = null;

            foreach (var token in this) {
                if (token.IsSelected) {
                    newStartChar = 
                        newStartChar == null ? 
                            token.Span.start : 
                            Math.Min(newStartChar.Value, token.Span.start);
                    newEndChar =
                        newEndChar == null ?
                            token.Span.stop:
                            Math.Min(newEndChar.Value, token.Span.stop);
                }
                newMaxTokenTypeID = Math.Max(newMaxTokenTypeID ?? 0, token.TokenTypeID);
            }

            this.NotifyChanged(ref selectionStartChar, newStartChar, OnPropertyChanged, nameof(SelectionStartChar));
            this.NotifyChanged(ref selectionEndChar, newEndChar, OnPropertyChanged, nameof(SelectionEndChar));
            this.NotifyChanged(ref maxTokenTypeID, newMaxTokenTypeID, OnPropertyChanged, nameof(MaxTokenTypeID));
        }

        private int? selectionStartChar;
        public int? SelectionStartChar {
            get => selectionStartChar;
            set {
                updateSelection(value, selectionEndChar);
                this.NotifyChanged(ref selectionStartChar, value, OnPropertyChanged);
            }
        }

        private int? selectionEndChar;
        public int? SelectionEndChar {
            get => selectionEndChar;
            set {
                updateSelection(selectionStartChar, value);
                this.NotifyChanged(ref selectionEndChar, value, OnPropertyChanged);
            }
        }

        private int? maxTokenTypeID;
        public int? MaxTokenTypeID => maxTokenTypeID;

        public int[] GetSelectedIndexes() => this.Where(x => x.IsSelected).Select(x => x.Index).ToArray();

        private Dictionary<int, Token> tokenByIndex;
        public Token GetByIndex(int index) {
            if (tokenByIndex == null) { tokenByIndex = this.ToDictionary(x => x.Index); }
            if (tokenByIndex.TryGetValue(index, out var token)) { return token; }
            return null;
        }

        public void Select(int? startChar, int? endChar) {
            updateSelection(startChar, endChar);
            this.NotifyChanged(ref selectionStartChar, startChar, OnPropertyChanged, nameof(SelectionStartChar));
            this.NotifyChanged(ref selectionEndChar, endChar, OnPropertyChanged, nameof(SelectionEndChar));
        }

        public void ClearSelection() => Select(null, null);
    }
}
