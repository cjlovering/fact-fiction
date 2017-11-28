import { 
    FETCHING_SIMILAR,
    RECEIVE_SIMILAR
} from '../constants/actionTypes';

/* sentence details */
export default (
    state = {
        isFetching: false,
        similarTokenIds: {}, // token id --> []
    },
    action
) => {
    switch(action.type) {
        case FETCHING_SIMILAR:
            return Object.assign({}, state, {isFetching: true})
        case RECEIVE_SIMILAR:
            const similarTokenIds = Object.assign({}, state.similarTokenIds, action.similarTokenIds);
            const isFetching = false;
            return { similarTokenIds, isFetching }
        default:
            return state;
    }
}