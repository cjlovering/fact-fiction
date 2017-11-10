
import { 
    POST_TEXT_ENTRY,
    RECIEVE_TEXT_ENTRY,
    INVALIDATE_TEXT_ENTRY
} from '../constants/actionTypes';

/* text entry */
export default (
    state = {
        isFetching: false,
        didInvalidate: false,
        textEntryTokens: []
    },
    action
) => {
    switch(action.type) {
        case INVALIDATE_TEXT_ENTRY:
            return Object.assign({}, state, {
                didInvalidate: true
            });
        case POST_TEXT_ENTRY:
            return Object.assign({}, state, {
                isFetching: true,
                didInvalidate: false
            });
        case RECIEVE_TEXT_ENTRY:
            return Object.assign({}, state, {
                isFetching: false,
                didInvalidate: false,
                textEntryTokens: action.textEntryTokens,
                lastUpdated: action.receivedAt
            });
        default:
            return state;
    }

}