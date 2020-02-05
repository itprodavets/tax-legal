import {AdditionalInfoRequest, Report} from "@/modules/cbc/models";
import _ from "lodash";
import {ActionContext, ActionTree} from "vuex";
import {ReportState} from '../report.state';
import {AdditionalInformationState} from "./additional-information.state";

export const actions: ActionTree<AdditionalInformationState, ReportState> = {
		list: async (
			action: ActionContext<AdditionalInformationState, ReportState>,
			request: AdditionalInfoRequest
		) => {
			let data = action.rootGetters["cbc/report/report"] as Report;
			if (data && data.id === request.reportId)
				action.commit("GET_ADDITIONAL_INFO_LIST", data.additionalInfo);
			else {
				data = _.find(action.rootGetters["cbc/report/reports"], (x: Report) => x.id === request.reportId);
				if (data)
					action.commit("GET_ADDITIONAL_INFO_LIST", data.additionalInfo);
				else
					action.commit("GET_ADDITIONAL_INFO_LIST", []);
			}
		}
	}
;
