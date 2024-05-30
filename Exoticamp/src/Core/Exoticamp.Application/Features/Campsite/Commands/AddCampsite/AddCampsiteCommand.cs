﻿using Exoticamp.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exoticamp.Application.Features.Campsite.Commands.AddCampsite
{
    public class AddCampsiteCommand:IRequest<Response<CampsiteDto>>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public bool Status { get; set; }
        public string TentType { get; set; }
        public bool? isActive { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovededDate { get; set; }
        public string? DeletededBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
