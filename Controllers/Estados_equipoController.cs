using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using prestasmosApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace apiPracticas.Controllers
{
    [ApiController] 
    public class Estados_equipoController : ControllerBase
    {
        private readonly practicasContext _contexto;

        public Estados_equipoController(practicasContext miContexto) {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo para retornar todos los reg. de la tabla de EQUIPOS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Estados_equipo")]
        public IActionResult Get(){
            IEnumerable<Estados_equipo> Estados_equipoList = from e in _contexto.Estados_equipo
                                               select e;
            if (Estados_equipoList.Count() > 0)
            {
                return Ok(Estados_equipoList);
            }      
            return NotFound();              
        } 

        /// <summary>
        /// Metodo para retornar UN reg. de la tabla EQUIPOS filtrado por ID
        /// </summary>
        /// <param name="id">Valor Entero del campo id_equipos</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Estados_equipo/{id}")]
        public IActionResult getbyId(int id)
        {
            Estados_equipo unEstados_equipo = (from e in _contexto.Estados_equipo
                                where e.id_estado_equipo == id //Filtro por ID
                                select e).FirstOrDefault();
            if( unEstados_equipo != null)
            {
                return Ok(unEstados_equipo);
            }

            return NotFound();
        }


        /// <summary>
        /// Este metodo retorna los reg. en la tabl de EQUIPOS que contengan el valor dado en el parametro.
        /// </summary>
        /// <param name="buscarNombre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Estados_equipo/buscarnombre/{buscarNombre}")]
        public IActionResult obtenerNombre(string buscarNombre)
        {
            IEnumerable<Estados_equipo> Estados_equipoPorNombre = from e in _contexto.Estados_equipo
                                                   where e.nombre.Contains(buscarNombre)
                                                   select e;
            if (Estados_equipoPorNombre.Count() > 0)
            {
                return Ok(Estados_equipoPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/Estados_equipo")]
        public IActionResult guardarEstados_equipo([FromBody] Estados_equipo Estados_equipoNuevo)
        {
            try
            {
                IEnumerable<Estados_equipo> Estados_equipoExiste = from e in _contexto.Estados_equipo
                                                    where e.nombre == Estados_equipoNuevo.nombre

                                                    select e;
                if(Estados_equipoExiste.Count()==0)
                {
                    _contexto.Estados_equipo.Add(Estados_equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(Estados_equipoNuevo);
                }
                return Ok(Estados_equipoExiste);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/Estados_equipo")]
        public IActionResult updateEstados_equipo([FromBody] Estados_equipo Estados_equipoAModificar)
        {
            Estados_equipo Estados_equipoExiste = (from e in _contexto.Estados_equipo
                                    where e.id_estado_equipo == Tipo_equipoAModificar.id_estado_equipo
                                    select e).FirstOrDefault();
            if(Estados_equipoExiste is null)
            {
                return NotFound();
            }

            Estados_equipoExiste.nombre = Estados_equipoAModificar.nombre;
            Estados_equipoExiste.descripcion = Estados_equipoAModificar.descripcion;
            Estados_equipoExiste.modelo = Estados_equipoAModificar.modelo;

            _contexto.Entry(Estados_equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(Estados_equipoExiste);

        }

    }
}