using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;
        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _apiResponse = new();
        }

        // All villas endpoint
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas() {
            try
            {
                _logger.LogInformation("Obtener listado completo de villas");

                IEnumerable<Villa> villaList = await _villaRepo.GetAll();

                _apiResponse.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }

        // One villa endpoint
        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id) {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al buscar el la villa con el ID [" + id + "]");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepo.GetOne(v => v.Id == id);

                if (villa == null)
                {
                    _logger.LogError("Error, no se encuentra la villa con el ID [" + id + "]");
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<VillaDto>(villa);
                _apiResponse.StatusCode = HttpStatusCode.OK;

                _logger.LogInformation("Retorna la villa correctamente");
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }

        // New villa endpoint
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Error, el modelostate recibido no es correcto.");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.GetOne(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
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

                modelo.dateCreate = DateTime.Now;
                modelo.dateUpdate = DateTime.Now;
                await _villaRepo.Create(modelo);
                //await _dbContext.SaveChangesAsync();

                _apiResponse.Result = modelo;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                _logger.LogInformation("Se registro la villa correctamente");
                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }

        //Delete villa endpoint
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVilla(int id) {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error, El ID recibido es 0.");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villa = await _villaRepo.GetOne(v => v.Id == id);

                if (villa == null)
                {
                    _logger.LogError("Error, no se recupera una villa con el ID" + id + ".");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                //VillaStore.villaList.Remove(villa);

                await _villaRepo.Remove(villa);
                //await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Se elimino la villa correctamente");

                _apiResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_apiResponse);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto) {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _logger.LogError("Error, con el objeto villa recibido.");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
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

                await _villaRepo.Update(modelo);
                //await _dbContext.SaveChangesAsync();

                _apiResponse.StatusCode = HttpStatusCode.NoContent;

                _logger.LogInformation("Se actualizo la villa correctamente");
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_apiResponse);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    _logger.LogError("Error, la propiedad de la villa recibido es incorrecto.");
                    return BadRequest();
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepo.GetOne(v => v.Id == id, tracked: false);

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

                await _villaRepo.Update(modelo);
                //await _dbContext.SaveChangesAsync();

                _apiResponse.StatusCode = HttpStatusCode.NoContent;

                _logger.LogInformation("Se actualizo la villa correctamente");
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_apiResponse);
        }
    }
}
