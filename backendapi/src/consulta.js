const express = require('express');  //para crear rutas
const router = express.Router();
const moment = require('moment-timezone');
const conec = require('../src/sqllib')

const configDB = {
    server: 'localhost', // Dirección IP del servidor SQL Server
    authentication: {
        type: 'default',
        options:{
            userName:"",
            password:""
        }
    },
    options:{
        port:1433,
        database:'bjjlog',
        trustServerCertificate: true
    }
  };

router.get('/', async (req, res) => {
   const total = [{"Hola":"1"},{"Hola":"2"}]
   res.json(total)
});

router.get('/identificacion/:id', async (req, res) => {
    try{
        var fechaActual = new Date();
        var formatoFecha = fechaActual.toISOString().slice(0, 10);
        const total = req.params.id;
        const resutl = await conec.ConsultaId(total);
        resutl.fechafin = calcularDiasHastaFecha(resutl.fechafin);
        console.log(resutl);
        console.log("------------------------");
        if(resutl.clase > 0)
        {
            if(resutl.cantidad > 0)
            {
                const mermar = await conec.MermarDia(resutl.Id,resutl.idmovimiento, resutl.cantidad)
                const insertAsistencia = await conec.IngresarAsistencia(resutl.Id,resutl.idplan,formatoFecha,mermar);
                resutl.Nombres = 'Nombre: ' + resutl.Nombres,
                resutl.Apellidos = 'Apellidos: ' + resutl.Apellidos,
                resutl.Identificacion = 'Identificacion: ' + resutl.Identificacion,
                resutl.fechafin = 'Clases restantes: ' + mermar,
                resutl.status = 'Estado: Activo',
                resutl.imagen = './approved.png'
            }
            else
            {
                resutl.Nombres = 'Nombre: ' + resutl.Nombres,
                resutl.Apellidos = 'Apellidos: ' + resutl.Apellidos,
                resutl.Identificacion = 'Identificacion: ' + resutl.Identificacion,
                resutl.fechafin = 'Clases restantes: ' + resutl.cantidad,
                resutl.status = 'Estado: No Activo',
                resutl.imagen = './desapproved.png'
            }
        }
        else
        {
            if(resutl.fechafin < 0)
            {
                resutl.Nombres = 'Nombre: ' + resutl.Nombres,
                resutl.Apellidos = 'Apellidos: ' + resutl.Apellidos,
                resutl.Identificacion = 'Identificacion: ' + resutl.Identificacion,
                resutl.fechafin = 'Dias restantes: 0',
                resutl.status = 'Estado: No Activo',
                resutl.imagen = './desapproved.png'
            }
            else
            {
                const insertAsistencia = await conec.IngresarAsistencia(resutl.Id,resutl.idplan,formatoFecha,0);
                resutl.Nombres = 'Nombre: ' + resutl.Nombres,
                resutl.Apellidos = 'Apellidos: ' + resutl.Apellidos,
                resutl.Identificacion = 'Identificacion: ' + resutl.Identificacion,
                resutl.fechafin = 'Dias restantes: ' + resutl.fechafin,
                resutl.status = 'Estado: Activo',
                resutl.imagen = './approved.png'
            }
        }
        res.json(resutl)
    }
    catch(error)
    {
        console.error(error);
        res.status(500).json({ error: 'Error al obtener los usuarios' });
    }

 });

 function calcularDiasHastaFecha(fechaFutura) {
    var fechaActual = new Date();
    var fechaProxima = new Date(fechaFutura);
    var diferenciaTiempo = fechaProxima - fechaActual;
    var diasFaltantes = Math.ceil(diferenciaTiempo / (1000 * 60 * 60 * 24));
    return diasFaltantes;
  }
  
module.exports = router;