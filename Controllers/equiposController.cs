using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using prestasmosApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace apiPracticas.Controllers
{
    [ApiController] 
    public class equiposController : ControllerBase
    {
        private readonly practicasContext _contexto;

        public equiposController(practicasContext miContexto) {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo para retornar todos los reg. de la tabla de EQUIPOS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipos")]
        public IActionResult Get(){
            IEnumerable<equipos> equiposList = from e in _contexto.equipos
                                               select e;
            if (equiposList.Count() > 0)
            {
                return Ok(equiposList);
            }      
            return NotFound();              
        } 

        /// <summary>
        /// Metodo para retornar UN reg. de la tabla EQUIPOS filtrado por ID
        /// </summary>
        /// <param name="id">Valor Entero del campo id_equipos</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipos/{id}")]
        public IActionResult getbyId(int id)
        {
            equipos unEquipo = (from e in _contexto.equipos
                                where e.id_equipos == id //Filtro por ID
                                select e).FirstOrDefault();
            if( unEquipo != null)
            {
                return Ok(unEquipo);
            }

            return NotFound();
        }


        /// <summary>
        /// Este metodo retorna los reg. en la tabl de EQUIPOS que contengan el valor dado en el parametro.
        /// </summary>
        /// <param name="buscarNombre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipo/buscarnombre/{buscarNombre}")]
        public IActionResult obtenerNombre(string buscarNombre)
        {
            IEnumerable<equipos> equipoPorNombre = from e in _contexto.equipos
                                                   where e.nombre.Contains(buscarNombre)
                                                   select e;
            if (equipoPorNombre.Count() > 0)
            {
                return Ok(equipoPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/equipos")]
        public IActionResult guardarEquipo([FromBody] equipos equipoNuevo)
        {
            try
            {
                IEnumerable<equipos> equipoExiste = from e in _contexto.equipos
                                                    where e.nombre == equipoNuevo.nombre

                                                    select e;
                if(equipoExiste.Count()==0)
                {
                    _contexto.equipos.Add(equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(equipoNuevo);
                }
                return Ok(equipoExiste);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/equipos")]
        public IActionResult updateEquipo([FromBody] equipos equipoAModificar)
        {
            equipos equipoExiste = (from e in _contexto.equipos
                                    where e.id_equipos == equipoAModificar.id_equipos
                                    select e).FirstOrDefault();
            if(equipoExiste is null)
            {
                return NotFound();
            }

            equipoExiste.nombre = equipoAModificar.nombre;
            equipoExiste.descripcion = equipoAModificar.descripcion;
            equipoExiste.modelo = equipoAModificar.modelo;

            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(equipoExiste);

        }

    }
}