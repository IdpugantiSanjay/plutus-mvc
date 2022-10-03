document.addEventListener('alpine:init', () => {
  Alpine.data('ListComponent', () => (({
    highlight: 0, init() {
      document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') this.highlight = 0
      })
    }, /**
     *
     * {MouseEvent} e
     */
    resetHighlight(e) {
      e.stopPropagation()
      e.preventDefault()
      this.highlight = 0
    }, /**
     *
     * {MouseEvent} e
     * {Number} itemId
     */
    listItemClick(e, itemId) {
      e.stopPropagation()
      e.preventDefault()
      this.highlight = itemId
    },
  })));
})

