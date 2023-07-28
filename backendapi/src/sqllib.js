import sql from 'mssql'
const dbsettings = {
    user:'checkmat',
    password: '1234',
    server: '127.0.0.1',
    database: 'bjjlog',
    options:{
        encrypt: true,
        trustServerCertificate : true
    }
}

var Modelos = function (e){
    this.Modelos = e.Modelos
}

Modelos.ConsultaId = ( async (id)=>{ 
    try 
    {
        const pool = await sql.connect(dbsettings)
        const consulta = "SELECT r.Id, r.Nombres, r.Apellidos, r.Identificacion, m.fechainicio,m.fechafin, m.idplan, p.clase, m.cantidad, m.Id as idmovimiento FROM registros r JOIN movimientos m ON r.Id = m.idregistro JOIN Planes p on m.idplan = p.id WHERE r.Identificacion = '" + id +"' and m.Id = (SELECT MAX(Id) FROM movimientos WHERE idregistro = r.Id)";
        const result =  await pool.request().query(consulta);
        return result.recordset[0];
    } catch (err) 
    {
        console.log(err)
    }
});

Modelos.IngresarAsistencia = ( async (id,idplan,fecha,cantidad)=>{ 
    try 
    {
        var fechaActual = new Date();
        var formatoFecha = fechaActual.toISOString().slice(0, 10);
        const pool = await sql.connect(dbsettings)
        const consulta = "select * from asistencia where idregistro = " + id + " and fecha = '" + formatoFecha + "'";
        const result =  await pool.request().query(consulta);
        console.log(result.recordset.length);
        if(result.recordset.length == 0)
        {
            const consulta2 = "Insert Into asistencia (idregistro, idplan, fecha, cantidad) VALUES ('" + id + "','" + idplan + "','" + fecha + "','" + cantidad + "')";
            const result2 =  await pool.request().query(consulta2);        
            return result2;
        }
        else
        {
           return ""
        }
    } catch (err) 
    {
        console.log(err)
    }
});

Modelos.MermarDia = ( async (idregis, idmove, days)=>{ 
    try 
    {
        var fechaActual = new Date();
        var formatoFecha = fechaActual.toISOString().slice(0, 10);
        const pool = await sql.connect(dbsettings)
        const consulta = "select * from asistencia where idregistro = " + idregis + " and fecha = '" + formatoFecha + "'";
        const result =  await pool.request().query(consulta);
        if(result.recordset.length == 0)
        {
            days = parseInt(days - 1)
            const consulta2 = "update movimientos set cantidad = '" + days + "' where Id = " + idmove;
            const result2 =  await pool.request().query(consulta2);
            return days;
        }
        else
        {
           return days
        }
        
    } catch (err) 
    {
        console.log(err)
    }
});

module.exports = Modelos;