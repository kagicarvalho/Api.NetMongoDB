using System;
using api.Data.Collections;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            this.mongoDB = mongoDB;
            _infectadosCollection = this.mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto infectadoDto)
        {
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == infectadoDto.DataNascimento), Builders<Infectado>.Update.Set("sexo", infectadoDto.Sexo));
            
            return Ok("Atualizado com Sucesso");
        }

        [HttpDelete("{dataNascimento}")]
        public ActionResult ExcluirInfectado(string dataNascimento)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == Convert.ToDateTime(dataNascimento)));
            return Ok("Atualizado com Sucesso");
        }
    }
}