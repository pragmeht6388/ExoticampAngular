﻿using AutoMapper;
using MediatR;
using MyCleanProject1.Application.Contracts.Persistence;
using MyCleanProject1.Application.Exceptions;
using MyCleanProject1.Application.Features.Categories.Commands.UpdateCategory;
using MyCleanProject1.Application.Response;
using MyCleanProject1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCleanProject1.Application.Features.Products.Commands.UpdateProduct
{
    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<UpdateProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;

        public UpdateProductCommandHandler(IMapper mapper, IProductRepository productRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _messageRepository = messageRepository;
        }

        public async Task<Response<UpdateProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productToUpdate = await _productRepository.GetByIdAsync(request.ProductId);

            if (productToUpdate == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            var validator = new UpdateProductCommandValidator(_messageRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult);

            _mapper.Map(request, productToUpdate);

            await _productRepository.UpdateAsync(productToUpdate);
            UpdateProductDto dto = new UpdateProductDto()
            {

                ProductId = request.ProductId,
                Name = request.Name,
                Price=request.Price
            };
            return new Response<UpdateProductDto>(dto, "Updated successfully ");

        }


    }
}
