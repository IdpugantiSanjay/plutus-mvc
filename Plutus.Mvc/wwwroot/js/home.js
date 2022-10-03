export const Home = {
  /**
   * @param {KeyboardEvent} e
   */
  numbersInput(e) {
    if (!e.target.valueAsNumber) {
      e.target.value = ''
    }
  }
}