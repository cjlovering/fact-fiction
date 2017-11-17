import { 
    FETCHING_TOKENS,    
    RECEIVE_TEXT_ENTRY,
    INVALIDATE_TEXT_ENTRY,
    RECEIVE_TOKENS,
    RECEIVE_FEED
} from '../constants/actionTypes';

/* text entry */
export default (
    state = {
        isFetching: false,          // is fetching text entry result
        didInvalidate: false,      
        textEntryTokenIds: [],      // a list of text entry token ids 
        tokens: {},                 // map of token id to token

        isDoneFetchingFeed: false,  // is done with fetching feed
        feedTokenIds: []            // a list of token ids for feed
    },
    action
) => {
    switch(action.type) {
        case INVALIDATE_TEXT_ENTRY:
            return Object.assign({}, state, {
                didInvalidate: true
            });
        case FETCHING_TOKENS:
            return Object.assign({}, state, {
                isFetching: true,
                didInvalidate: false
            });
        case RECEIVE_TOKENS:
            return Object.assign({}, state, {
                isFetching: false,
                didInvalidate: false,
                tokens: Object.assign({}, state.tokens, action.tokens),
                lastUpdated: action.receivedAt
            });
        case RECEIVE_TEXT_ENTRY:
            return Object.assign({}, state, {
                textEntryTokenIds: action.textEntryTokenIds
            })
        case RECEIVE_FEED:
            console.log(action.feedTokenIds);
            return Object.assign({}, state, {
                isDoneFetchingFeed: action.feedTokenIds.length == 0,
                feedTokenIds: [...state.feedTokenIds, ...action.feedTokenIds]
            })
        default:
            return state;
    }

}