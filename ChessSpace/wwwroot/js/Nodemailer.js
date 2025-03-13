const nodemailer = require('nodemailer');

// Maak een transport object (hier gebruiken we Gmail als voorbeeld)
const transporter = nodemailer.createTransport({
    service: 'gmail',
    auth: {
        user: 'jouw-email@gmail.com',  // Je Gmail-adres
        pass: 'jouw-wachtwoord'        // Je Gmail-wachtwoord (of app-specifieke wachtwoord)
    }
});

// Genereer een 6-cijferige code
const generateCode = () => Math.floor(100000 + Math.random() * 900000);  // Bijvoorbeeld: 123456

const code = generateCode();

// Mail-opties
const mailOptions = {
    from: 'jouw-email@gmail.com',
    to: 'ontvanger@example.com',
    subject: 'Je verificatiecode',
    text: `Je 6-cijferige code is: ${code}`
};

// Verstuur de e-mail
transporter.sendMail(mailOptions, (error, info) => {
    if (error) {
        console.log('Fout bij het verzenden:', error);
    } else {
        console.log('E-mail verzonden:', info.response);
    }
});
