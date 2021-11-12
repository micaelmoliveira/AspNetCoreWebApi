using Api.DTOs;
using AutoMapper;
using Business.Intefaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoservice;
        private readonly IMapper _mapper;



        public ProdutosController(IProdutoRepository produtoRepository,
                                  IProdutoService produtoservice,
                                  IMapper mapper,
                                  INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoservice = produtoservice;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoDTO>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.ObterTodos());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoDTO>> ObterPorId(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto == null) return NotFound();

            return produto;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Adicionar(ProdutoDTO produtoDTO)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = new Guid() + "_" + produtoDTO.Imagem;

            if(!UploadArquivo(produtoDTO.Imagem, imagemNome))
            {
                return CustomResponse();
            }

            produtoDTO.Imagem = imagemNome;
            await _produtoservice.Adicionar(_mapper.Map<Produto>(produtoDTO));

            return CustomResponse(produtoDTO);
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoDTO>> Excluir(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto != null) return NotFound();

            await _produtoservice.Remover(id);

            return CustomResponse();
        }

        private async Task<ProdutoDTO> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterProdutoFornecedor(id));
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            var imageDataByteArray = Convert.FromBase64String(arquivo);

            if(string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }
    }
}
