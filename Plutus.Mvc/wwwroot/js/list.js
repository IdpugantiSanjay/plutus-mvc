document.addEventListener('alpine:init', () => {
  Alpine.data('ListComponent', () => (({
    highlight: 0,
    showFilters: false,

    init() {
      const searchParams = new URL(document.location).searchParams
      this.showFilters = searchParams.get('query') || searchParams.get('sortBy')
    },

    /**
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

    onFilterIconClick() {
      this.showFilters = !this.showFilters
      if (this.showFilters) {
        setTimeout(() => document.querySelector('input[name="query"]').focus(), 150)
      }
    },

  })));
})

