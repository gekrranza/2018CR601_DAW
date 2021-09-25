using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using prestasmosApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace apiPracticas.Controllers
{
    [ApiController] 
    public class Tipo_equipoController : ControllerBase
    {
        private readonly practicasContext _contexto;

        public Tipo_equipoController(practicasContext miContexto) {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo para retornar todos los reg. de la tabla de EQUIPOS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Tipo_equipo")]
        public IActionResult Get(){
            IEnumerable<Tipo_equipo> Tipo_equipoList = from e in _contexto.Tipo_equipo
                                               select e;
            if (Tipo_equipoList.Count() > 0)
            {
                return Ok(Tipo_equipoList);
            }      
            return NotFound();              
        } 

        /// <summary>
        /// Metodo para retornar UN reg. de la tabla EQUIPOS filtrado por ID
        /// </summary>
        /// <param name="id">Valor Entero del campo id_equipos</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Tipo_equipo/{id}")]
        public IActionResult getbyId(int id)
        {
            Tipo_equipo unTipo_equipo = (from e in _contexto.Tipo_equipo
                                where e.id_tipo_equipo == id //Filtro por ID
                                select e).FirstOrDefault();
            if( unTipo_equipo != null)
            {
                return Ok(unTipo_equipo);
            }

            return NotFound();
        }


        /// <summary>
        /// Este metodo retorna los reg. en la tabl de EQUIPOS que contengan el valor dado en el parametro.
        /// </summary>
        /// <param name="buscarNombre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Tipo_equipo/buscarnombre/{buscarNombre}")]
        public IActionResult obtenerNombre(string buscarNombre)
        {
            IEnumerable<Tipo_equipo> Tipo_equipoPorNombre = from e in _contexto.Tipo_equipo
                                                   where e.nombre.Contains(buscarNombre)
                                                   select e;
            if (Tipo_equipoPorNombre.Count() > 0)
            {
                return Ok(Tipo_equipoPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/Tipo_equipo")]
        public IActionResult guardarEquipo([FromBody] Tipo_equipo Tipo_equipoNuevo)
        {
            try
            {
                IEnumerable<Tipo_equipo> Tipo_equipoExiste = from e in _contexto.Tipo_equipo
                                                    where e.nombre == Tipo_equipoNuevo.nombre

                                                    select e;
                if(Tipo_equipoExiste.Count()==0)
                {
                    _contexto.Tipo_equipo.Add(Tipo_equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(Tipo_equipoNuevo);
                }
                return Ok(Tipo_equipoExiste);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/Tipo_equipo")]
        public IActionResult updateTipo_equipo([FromBody] Tipo_equipo Tipo_equipoAModificar)
        {
            Tipo_equipo Tipo_equipoExiste = (from e in _contexto.Tipo_equipo
                                    where e.id_tipo_equipo == Tipo_equipoAModificar.id_tipo_equipo
                                    select e).FirstOrDefault();
            if(Tipo_equipoExiste is null)
            {
                return NotFound();
            }

            Tipo_equipoExiste.nombre = Tipo_equipoAModificar.nombre;
            Tipo_equipoExiste.descripcion = Tipo_equipoAModificar.descripcion;
            Tipo_equipoExiste.modelo = Tipo_equipoAModificar.modelo;

            _contexto.Entry(Tipo_equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(Tipo_equipoExiste);

        }

    }
}