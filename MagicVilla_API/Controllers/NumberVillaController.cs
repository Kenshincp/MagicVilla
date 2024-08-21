using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberVillaController : ControllerBase
    {
        private readonly ILogger<NumberVillaController> _logger;
        private readonly INumberVillaRepository _numberRepo;
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;
        public NumberVillaController(ILogger<NumberVillaController> logger, IVillaRepository villaRepo, INumberVillaRepository numberRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numberRepo = numberRepo;
            _mapper = mapper;
            _apiResponse = new();
        }

        // All villas endpoint
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetNumberVillas() {
            try
            {
                _logger.LogInformation("Obtener listado completo de nùmeros de villas");

                IEnumerable<NumberVilla> numberVillaList = await _numberRepo.GetAll();

                _apiResponse.Result = _mapper.Map<IEnumerable<NumberVillaDto>>(numberVillaList);
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
        [HttpGet("id:int", Name = "GetNumberVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumberVilla(int id) {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al buscar el nùmero de la villa con el ID [" + id + "]");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var numberVilla = await _numberRepo.GetOne(v => v.VillaNo == id);

                if (numberVilla == null)
                {
                    _logger.LogError("Error, no se encuentra el nùmero de la villa con el ID [" + id + "]");
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<NumberVillaDto>(numberVilla);
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
        public async  Task<ActionResult<APIResponse>> CreateNumberVilla([FromBody] NumberVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Error, el modelostate recibido no es correcto.");
                    return BadRequest(ModelState);
                }

                if (await _numberRepo.GetOne(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    _logger.LogError("Error, el nùmero de la villa ya existe en la base de datos.");
                    ModelState.AddModelError("NameExist", "The number is exist");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    _logger.LogError("Error, numberVillaDto llega nulo.");
                    return BadRequest(createDto);
                }

                if (await _villaRepo.GetOne(v => v.Id == createDto.VillaId) == null)
                {
                    _logger.LogError("Error, el ID de la villa no existe en la base de datos.");
                    ModelState.AddModelError("VillaId", "The ID is not exist");
                    return BadRequest(ModelState);
                }

                //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

                //VillaStore.villaList.Add(villaDto);

                NumberVilla modelo = _mapper.Map<NumberVilla>(createDto);

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
                await _numberRepo.Create(modelo);
                //await _dbContext.SaveChangesAsync();

                _apiResponse.Result = modelo;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                _logger.LogInformation("Se registro la villa correctamente");
                return CreatedAtRoute("GetNumberVilla", new { id = modelo.VillaNo }, _apiResponse);
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
        public async Task<IActionResult> DeleteNumberVilla(int id) {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error, El ID recibido es 0.");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var numberVilla = await _numberRepo.GetOne(v => v.VillaNo == id);

                if (numberVilla == null)
                {
                    _logger.LogError("Error, no se recupera el nùmero de la villa con el nùmero" + id + ".");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                //VillaStore.villaList.Remove(villa);

                await _numberRepo.Remove(numberVilla);
                //await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Se elimino el nùmero de la villa correctamente");

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
        public async Task<IActionResult> NumberUpdateVilla(int id, [FromBody] NumberVillaUpdateDto updateDto) {
            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    _logger.LogError("Error, con el objeto villa recibido.");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                if (await _villaRepo.GetOne(v => v.Id == updateDto.VillaId) == null)
                {
                    _logger.LogError("Error, el ID de la villa no existe en la base de datos.");
                    ModelState.AddModelError("VillaId", "The ID is not exist");
                    return BadRequest(ModelState);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

                //villa.Name = villaDto.Name;
                //villa.Population = villaDto.Population;
                //villa.SquareMeter = villaDto.SquareMeter;

                NumberVilla modelo = _mapper.Map<NumberVilla>(updateDto);


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

                await _numberRepo.Update(modelo);
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
