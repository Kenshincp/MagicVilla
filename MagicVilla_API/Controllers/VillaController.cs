using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly AplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, AplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // All villas endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas() {
            _logger.LogInformation("Obtener listado completo de villas");

            IEnumerable<Villa> villaList = await _dbContext.Villas.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }

        // One villa endpoint
        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id) {
            if (id == 0)
            {
                _logger.LogError("Error al buscar el la villa con el ID [" + id + "]");
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogError("Error, no se encuentra la villa con el ID [" + id + "]");
                return NotFound();
            }

            _logger.LogInformation("Retorna la villa correctamente");
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        // New villa endpoint
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Error, el modelostate recibido no es correcto.");
                return BadRequest(ModelState);
            }

            if (await _dbContext.Villas.FirstOrDefaultAsync(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                _logger.LogError("Error, el nombre de la villa ya existe en la base de datos.");
                ModelState.AddModelError("NameExist", "The name is exist");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                _logger.LogError("Error, villaDto llega nulo.");
                return BadRequest(createDto);
            }

            //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

            //VillaStore.villaList.Add(villaDto);

            Villa modelo = _mapper.Map<Villa>(createDto);

            /* REMPLAZAMOS LA FORMA DE MAPEAR LOS DATOS:
            Villa modelo = new()
            {
                Name = villaDto.Name,
                Description = villaDto.Description,
                ImageUrl = villaDto.ImageUrl,
                Population = villaDto.Population,
                Amenity = villaDto.Amenity,
                SquareMeter = villaDto.SquareMeter,
                Rate = villaDto.Rate,
            };
            */

            await _dbContext.Villas.AddAsync(modelo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Se registro la villa correctamente");
            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }

        //Delete villa endpoint
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVilla(int id) {
            if (id == 0)
            {
                _logger.LogError("Error, El ID recibido es 0.");
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogError("Error, no se recupera una villa con el ID" + id + ".");
                return NotFound();
            }

            //VillaStore.villaList.Remove(villa);

            _dbContext.Villas.Remove(villa);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Se elimino la villa correctamente");
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto) {
            if (updateDto == null || id != updateDto.Id)
            {
                _logger.LogError("Error, con el objeto villa recibido.");
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            //villa.Name = villaDto.Name;
            //villa.Population = villaDto.Population;
            //villa.SquareMeter = villaDto.SquareMeter;

            Villa modelo = _mapper.Map<Villa>(updateDto);


            /* REMPLAZAMOS LA FORMA DE MAPEAR LOS DATOS:
            Villa modelo = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Description = villaDto.Description,
                ImageUrl = villaDto.ImageUrl,
                Population = villaDto.Population,
                Amenity = villaDto.Amenity,
                SquareMeter = villaDto.SquareMeter,
                Rate = villaDto.Rate,
            };
            */ 

            _dbContext.Villas.Update(modelo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Se actualizo la villa correctamente");
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogError("Error, la propiedad de la villa recibido es incorrecto.");
                return BadRequest();
            }
                
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            /* REMPLAZAMOS LA FORMA DE MAPEAR LOS DATOS:
            VillaUpdateDto villaDto = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Description = villa.Description,
                ImageUrl = villa.ImageUrl,
                Population = villa.Population,
                Amenity = villa.Amenity,
                SquareMeter = villa.SquareMeter,
                Rate = villa.Rate,
            };
            */

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Error, la propiedad de la villa a ser editada, no se conrresponde con el modelstate correcto.");
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            /* REMPLAZAMOS LA FORMA DE MAPEAR LOS DATOS:
            Villa modelo = new()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Description = villaDto.Description,
                ImageUrl = villaDto.ImageUrl,
                Population = villaDto.Population,
                Amenity = villaDto.Amenity,
                SquareMeter = villaDto.SquareMeter,
                Rate = villaDto.Rate,
            };
            */

            _dbContext.Villas.Update(modelo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Se actualizo la villa correctamente");
            return NoContent();
        }
    }
}
