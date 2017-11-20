import { SHOW_DETAILS } from '../constants/actionTypes';
export default (state = {}, action) => {
    switch(action.type) {
        case SHOW_DETAILS:
            let update = {}
            const id = action.id;            
            update[id] = action.show;
            return Object.assign({}, state, update);
        default:
            return state;
    }
}
