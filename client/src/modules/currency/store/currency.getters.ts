import {GetterTree} from "vuex";
import {CurrencyState} from "./currency.state";
import {RootState} from '@/core/store/root.state';

export const getters: GetterTree<CurrencyState, RootState> = {
  currencies: (state: CurrencyState) => state.entities
};
export default getters;
