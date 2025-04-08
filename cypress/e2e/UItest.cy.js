describe('UI tests', () => {
  it('login-logout flow', () => {
    cy.visit('https://localhost:7238')
    cy.contains('h1', 'Welcome')
    cy.contains("button", 'Log in').click()
    cy.url().should('include', '/Login')
    cy.get('input[name="email"]').type('chessspacenoreply@gmail.com')
    cy.get('input[name="password"]').type('onbelangRyk!0')
    cy.get('button[type="submit"]').click()
    cy.url().should('include', '/Home')
    cy.contains('Welcome Joshua Huigen')
    cy.contains('button', 'Log Out').click()
    cy.contains("button", 'Log in')
  })

  it('register-goback flow', () => {
    cy.visit('https://localhost:7238')
    cy.contains("button", 'Create an Account').click()
    cy.url().should('include', '/Register')
    cy.contains('button', 'Go Back').click()
    cy.contains("button", 'Log in')
  })
})