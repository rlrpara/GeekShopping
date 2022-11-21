using AutoMapper;
using GeekShoping.Shared.Services.Interface;
using GeekShoping.Shared.Services.Service;
using GeekShoping.Shared.Services.ViewModel;
using GeekShoping.Shared.Services.ViewModel.filtro;
using GeekShopping.ProductApi.Model;
using GeekShopping.Shared.Data.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace GeekShopping.ProductApi.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    #region [Propriedades Privadas]
    private readonly IMapper _mapper;
    private readonly IProductService _service;
    #endregion

    #region [Métodos Privados]
    private int ObterTotalPaginas(filtroProductViewModel filtro)
    {
        var total = _service.ObterTotalRegistros(filtro) / filtro.QuantityPerPage;
        if ((_service.ObterTotalRegistros(filtro) % filtro.QuantityPerPage) > 0)
            total += 1;
        return total.Equals(0) ? 1 : total;
    }
    #endregion

    #region [Contrutor]
    public ProductController(IMapper mapper)
    {
        _mapper = mapper;
        _service = new ProductService(new BaseRepository(), _mapper);
    }
    #endregion

    #region [Propriedades Públicas]
    [HttpPost("GetAll")]
    public IActionResult PostGetAll([FromBody] filtroProductViewModel filtro)
    {
        var dadosRetorno = _service.ObterTodos(filtro).ToList();

        if (dadosRetorno is null)
            return Ok(new { Resultado = "Registro não encontrado." });

        var resultado = new ApiResult<ProductViewModel>();
        resultado.AddPaginacao(filtro.ActualPage, filtro.QuantityPerPage, ObterTotalPaginas(filtro), _service.ObterTotalRegistros(filtro), dadosRetorno);

        return Ok(resultado);
    }

    [HttpPost("Insert")]
    public IActionResult PostInsert([FromBody] ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (_service.ObterEntidade(model))
                return Ok(new { Resultado = "Registro já cadastrado." });

            return Created("", _service.Inserir(model));
        }
        return BadRequest(ModelState);
    }

    [HttpPut("Update")]
    public IActionResult PutUpdate([FromBody] ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Codigo.Equals(0))
                return Ok(new { Resultado = "Registro não encontrado" });

            var consulta = _service.ObterPorCodigo(model.Codigo);

            if (consulta is not null)

                return Ok(_service.Atualizar(model));
            else
                return BadRequest(new { Resultado = "Registro não encontrado" });
        }

        return BadRequest(ModelState);
    }

    [HttpDelete("Remove")]
    public IActionResult DeleteRemove(int Codigo)
    {
        if (Codigo.Equals(0))
            return Ok(new { Resultado = "Registro não encontrado" });

        var consulta = _service.ObterPorCodigo(Codigo);

        if (consulta.Ativo is not null && !(consulta.Ativo ?? false))
            return Ok(new { Resultado = "Registro ja deletado" });

        if (consulta is not null)
            return Ok(_service.Deletar(consulta));
        else
            return BadRequest(new { Resultado = "Registro não encontrado" });
    }

    #endregion
}
