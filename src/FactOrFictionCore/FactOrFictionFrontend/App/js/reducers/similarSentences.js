import { 
    FETCHING_SIMILAR,
    RECEIVE_SIMILAR
} from '../constants/actionTypes';

/* sentence details */
export default (
    state = {
        isFetchingSimilar: false,
        similarSentenceIds: {}, // token id --> []
    },
    action
) => {
    switch(action.type) {
        case FETCHING_SIMILAR:
            return Object.assign({}, state, {isFetchingSimilar: true})
        case RECEIVE_SIMILAR:
            const similarSentenceIds = Object.assign({}, state.similarSentenceIds, action.similarSentenceIds);
            const isFetchingSimilar = false;
            return { similarSentenceIds, isFetchingSimilar }
        default:
            return state;
    }
}