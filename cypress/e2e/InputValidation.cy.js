describe('template spec', () => {
  it('Logt niet in bij lege velden', () => {
    cy.visit('https://localhost:7238/LoginRegister/Login?');
    cy.get('button[type="submit"]').click();
    cy.url().should('include', '/LoginRegister/Login?');
  });

  it('Joint geen game bij ongeldige code', () => {
    cy.visit('https://localhost:7238/Game/Joingame');
    cy.get('input[name="gameCode"]').type('12345678'); 
    cy.get('button[type="submit"]').click();
    cy.url().should('include', '/Game/Joingame');
    cy.contains('Game not found!'); 
  });
})