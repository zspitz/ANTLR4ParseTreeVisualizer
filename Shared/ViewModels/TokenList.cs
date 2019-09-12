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
        private void updateTokenIsSelected(int? startTokenIndex, int? endTokenIndex) {
            inUpdateSelection = true;
            foreach (var token in this) {
                if (startTokenIndex != null && endTokenIndex != null) {
                    token.IsSelected = token.Index >= startTokenIndex && token.Index <= endTokenIndex;
                } else {
                    token.IsSelected = false;
                }
            }
            inUpdateSelection = false;
        }

        private void recalculateProperties() {
            int? newSelectionStartTokenIndex = null;
            int? newSelectionEndTokenIndex = null;
            int? newMaxTokenTypeID = null;

            foreach (var token in this) {
                if (token.IsSelected) {
                    newSelectionStartTokenIndex = Math.Min(newSelectionStartTokenIndex ?? 0, token.Index);
                    newSelectionEndTokenIndex = Math.Min(newSelectionEndTokenIndex ?? 0, token.Index);
                }
                newMaxTokenTypeID = Math.Max(newMaxTokenTypeID ?? 0, token.TokenTypeID);
            }

            this.NotifyChanged(ref selectionStartTokenIndex, newSelectionStartTokenIndex, OnPropertyChanged);
            this.NotifyChanged(ref selectionEndTokenIndex, newSelectionEndTokenIndex, OnPropertyChanged);
            this.NotifyChanged(ref maxTokenTypeID, newMaxTokenTypeID, OnPropertyChanged);
        }

        private int? selectionStartTokenIndex;
        public int? SelectionStartTokenIndex {
            get => selectionStartTokenIndex;
            set {
                updateTokenIsSelected(value, selectionEndTokenIndex);
                this.NotifyChanged(ref selectionStartTokenIndex, value, OnPropertyChanged, "SelectionStartTokenIndex");

            }
        }

        private int? selectionEndTokenIndex;
        public int? SelectionEndTokenIndex {
            get => selectionEndTokenIndex;
            set {
                updateTokenIsSelected(selectionStartTokenIndex, value);
                this.NotifyChanged(ref selectionEndTokenIndex, value, OnPropertyChanged);
            }
        }

        private int? maxTokenTypeID;
        public int? MaxTokenTypeID => maxTokenTypeID;

        public int[] GetSelectedIndexes() => this.Where(x => x.IsSelected).Select(x => x.Index).ToArray();

        private Dictionary<int, Token> tokenByIndex;
        public Token GetByIndex(int index) {
            if (tokenByIndex == null) { tokenByIndex = this.ToDictionary(x => x.Index); }
            return tokenByIndex[index];
        }
    }
}
