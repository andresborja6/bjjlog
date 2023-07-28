const express = require('express');
const morgan = require('morgan');
const path = require('path');
const fileUpload = require('express-fileupload');
const bodyParser = require('body-parser');
const ip = require("ip");

// Servidor
const app = express();

// Configuración del puerto
app.set('port', process.env.PORT || 3000);

// Middlewares
app.use(morgan('dev'));
app.use(express.json());
app.use(fileUpload());

app.use((req, res, next) => {
  // Configuración de CORS
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Headers', 'Authorization, X-API-KEY, Origin, X-Requested-With, Content-Type, Accept, Access-Control-Allow-Request-Method');
  res.header('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, DELETE');
  res.header('Allow', 'GET, POST, OPTIONS, PUT, DELETE');
  next();
});

// Variables globales
app.use((req, res, next) => {
  next();
});

// Rutas
app.use('/crud', require('./src/consulta.js'));

// Ruta para servir el archivo HTML en la raíz del servidor
app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'Validacion.html'));
});

// Servir los archivos estáticos desde el directorio "public"
app.use(express.static(path.join(__dirname, 'public')));

app.listen(app.get('port'), () => {
  console.log(`Servidor desde el puerto ${app.get('port')}`);
  console.dir(ip.address());
});

module.exports = app;
