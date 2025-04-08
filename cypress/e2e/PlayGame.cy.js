let gameCode = '';

describe('Game join test', () => {
  it('Creëert een game en joint met een andere speler', () => {
    cy.visit('https://localhost:7238/Game/Creategame');

    cy.get('strong').invoke('text').then((text) => {
      gameCode = text.trim();
      cy.log('Gamecode is: ' + gameCode);
      cy.clearCookies();
      cy.clearLocalStorage();

      cy.visit('https://localhost:7238/Game/Joingame');
      cy.get('input[name="gameCode"]').type(gameCode); 
      cy.get('button[type="submit"]').click()

      cy.contains('Game Code: ' + gameCode); 
      cy.get('#E2 .chessbutton').click();
      cy.get('#E4 .chessbutton').click();
      cy.get('#E4 .chessbutton').should('have.class', 'orangepawn'); 
    });
  });
});
