import { 
    FETCHING_SIMILAR,
    RECEIVE_SIMILAR
} from '../constants/actionTypes';

/* sentence details */
export default (
    state = {
        isFetchingSimilar: false,
        similarTokenIds: {}, // token id --> []
    },
    action
) => {
    switch(action.type) {
        case FETCHING_SIMILAR:
            return Object.assign({}, state, {isFetchingSimilar: true})
        case RECEIVE_SIMILAR:
            const similarTokenIds = Object.assign({}, state.similarTokenIds, action.similarTokenIds);
            const isFetchingSimilar = false;
            return { similarTokenIds, isFetchingSimilar }
        default:
            return state;
    }
}