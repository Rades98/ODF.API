﻿using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Lineup
{
	public class DeleteLineupResponseModel : BaseCreateResponseModel
	{
		public DeleteLineupResponseModel(string message) : base(message)
		{
		}

		public DeleteLineupResponseModel(Form form, string message) : base(form, message)
		{
		}

		public DeleteLineupResponseModel(Form form) : base(form)
		{
		}
	}
}
