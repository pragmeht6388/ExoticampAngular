﻿using Exoticamp.Application.Contracts.Persistence;
using Exoticamp.Application.Helper;
using FluentValidation;
using Exoticamp.Application.Features.Events.Commands.UpdateEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exoticamp.Application.Features.Categories.Commands.UpdateCategory
{
    internal class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IMessageRepository _messageRepository;
        public UpdateCategoryCommandValidator(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage(GetMessage("1", ApplicationConstants.LANG_ENG))
                .NotNull()
                .MaximumLength(50).WithMessage(GetMessage("2", ApplicationConstants.LANG_ENG));

            //RuleFor(p => p.Price)
            //    .NotEmpty().WithMessage(GetMessage("1", ApplicationConstants.LANG_ENG))
            //    .GreaterThan(0);
        }

        private string GetMessage(string Code, string Lang)
        {
            return _messageRepository.GetMessage(Code, Lang).Result.MessageContent.ToString();
        }
    }
}
