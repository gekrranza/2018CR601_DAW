using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using prestasmosApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace apiPracticas.Controllers
{
    [ApiController] 
    public class marcasController : ControllerBase
    {
        private readonly practicasContext _contexto;

        public marcasController(practicasContext miContexto) {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo para retornar todos los reg. de la tabla de EQUIPOS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/marcas")]
        public IActionResult Get(){
            IEnumerable<marcas> equiposList = from e in _contexto.marcas
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
        [Route("api/marcas/{id}")]
        public IActionResult getbyId(int id)
        {
            marcas unMarca = (from e in _contexto.marcas
                                where e.id_marcas == id //Filtro por ID
                                select e).FirstOrDefault();
            if( unMarca != null)
            {
                return Ok(unMarca);
            }

            return NotFound();
        }


        /// <summary>
        /// Este metodo retorna los reg. en la tabl de EQUIPOS que contengan el valor dado en el parametro.
        /// </summary>
        /// <param name="buscarNombre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/marca/buscarnombre/{buscarNombre}")]
        public IActionResult obtenerNombre(string buscarNombre)
        {
            IEnumerable<marcas> marcaPorNombre = from e in _contexto.marcas
                                                   where e.nombre.Contains(buscarNombre)
                                                   select e;
            if (marcaPorNombre.Count() > 0)
            {
                return Ok(marcaPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/marcas")]
        public IActionResult guardarMarca([FromBody] marcas marcaNuevo)
        {
            try
            {
                IEnumerable<marcas> marcaExiste = from e in _contexto.marcas
                                                    where e.nombre == marcaNuevo.nombre

                                                    select e;
                if(marcaExiste.Count()==0)
                {
                    _contexto.equipos.Add(marcaNuevo);
                    _contexto.SaveChanges();
                    return Ok(marcaNuevo);
                }
                return Ok(marcaExiste);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/marcas")]
        public IActionResult updateMarca([FromBody] marcas marcaAModificar)
        {
            marcas marcaExiste = (from e in _contexto.marcas
                                    where e.id_marcas == marcaAModificar.id_marcas
                                    select e).FirstOrDefault();
            if(marcaExiste is null)
            {
                return NotFound();
            }

            marcaExiste.nombre = marcaAModificar.nombre;
            marcaExiste.descripcion = marcaAModificar.descripcion;
            marcaExiste.modelo = marcaAModificar.modelo;

            _contexto.Entry(marcaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(marcaExiste);

        }

    }
}