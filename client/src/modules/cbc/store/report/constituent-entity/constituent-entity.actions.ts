import {Guid} from "@/core/common/guid";
import {ConstituentEntityCreateRequest, ConstituentEntityRequest, ConstituentEntityUpdateRequest, Report} from "@/modules/cbc/models";
import _ from "lodash";
import {ActionContext, ActionTree} from "vuex";
import {ReportState} from "../report.state";
import {ConstituentEntityState} from "./constituent-entity.state";

export const actions: ActionTree<ConstituentEntityState, ReportState> = {
	list: async (
		action: ActionContext<ConstituentEntityState, ReportState>,
		request: ConstituentEntityRequest
	) => {
		let data = action.rootGetters["cbc/report/report"] as Report;
		if (data && data.id === request.reportId)
			action.commit("GET_CONSTITUENT_ENTITY_LIST", data.constituentEntities || []);
		else {
			data = _.find(action.rootGetters["cbc/report/reports"], (x: Report) => x.id === request.reportId);
			if (data)
				action.commit("GET_CONSTITUENT_ENTITY_LIST", data.constituentEntities || []);
			else
				action.commit("GET_CONSTITUENT_ENTITY_LIST", []);
		}
	},
	get: async (
		action: ActionContext<ConstituentEntityState, ReportState>,
		id: string | number
	) => {
		action.commit("GET_CONSTITUENT_ENTITY", id);
	},
	create: async (
		action: ActionContext<ConstituentEntityState, ReportState>,
		request: ConstituentEntityCreateRequest
	) => {
		request.constituentEntity.id = Guid.create().toString();
		action.commit("CREATE_CONSTITUENT_ENTITY", request.constituentEntity);
		return request.constituentEntity.id;
	},
	update: async (
		action: ActionContext<ConstituentEntityState, ReportState>,
		request: ConstituentEntityUpdateRequest
	) => {
		action.commit("UPDATE_CONSTITUENT_ENTITY", request.constituentEntity);
	}
};
