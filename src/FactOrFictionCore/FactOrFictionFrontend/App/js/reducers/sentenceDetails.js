import { 
    FETCHING_DETAILS,
    RECEIVE_DETAILS
} from '../constants/actionTypes';

/* sentence details */
export default (
    state = {
        isFetching: false,
        details: {}, // token id --> { entities: [], references: [] }
    },
    action
) => {
    switch(action.type) {
        case FETCHING_DETAILS:
            return Object.assign({}, state, {isFetching: true})
        case RECEIVE_DETAILS:
            const details = Object.assign({}, state.details, action.details);
            const isFetching = false;
            return { details, isFetching }
        default:
            return state;
    }
}