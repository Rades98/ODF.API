using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Base
{
	internal interface IBaseResponseModel
	{
		AppAction Self { get; set; }

		List<AppAction> Actions { get; }

		void AddAction(AppAction action);
	}
}
