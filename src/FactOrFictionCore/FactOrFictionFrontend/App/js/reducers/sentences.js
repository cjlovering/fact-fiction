import { 
    FETCHING_SENTENCES,    
    RECEIVE_TEXT_ENTRY,
    INVALIDATE_TEXT_ENTRY,
    RECEIVE_SENTENCES,
    RECEIVE_FEED,
    RECEIVE_VOTES
} from '../constants/actionTypes';

/* sentences */
export default (
    state = {
        isFetching: false,          // is fetching text entry result
        didInvalidate: false,      
        textEntrySentenceIds: [],   // a list of text entry sentence ids 
        sentences: {},              // map of sentence id to sentence
        isDoneFetchingFeed: false,  // is done with fetching feed
        feedSentenceIds: [],        // a list of sentence ids for feed
        votes: {}                   // map of sentence id and vote type - "TRUE", "FALSE" or "UNVOTED"
    },
    action
) => {
    switch(action.type) {
        case INVALIDATE_TEXT_ENTRY:
            return Object.assign({}, state, {didInvalidate: true})
        case FETCHING_SENTENCES:
            return Object.assign({}, state, {
                isFetching: true,
                didInvalidate: false
            });
        case RECEIVE_SENTENCES:
            return Object.assign({},
                state, 
                {
                    isFetching: false,
                    didInvalidate: false,
                    sentences: Object.assign({}, state.sentences, action.sentences)
                }
            );
        case RECEIVE_TEXT_ENTRY:
            return Object.assign(
                {},
                state, 
                { textEntrySentenceIds: action.textEntrySentenceIds }
            );
        case RECEIVE_FEED:
            return Object.assign({}, state, {
                isDoneFetchingFeed: action.feedSentenceIds.length == 0,
                feedSentenceIds: [...state.feedSentenceIds, ...action.feedSentenceIds]
            });
        case RECEIVE_VOTES:
            return Object.assign({}, state, {
                votes: Object.assign({}, state.votes, action.votes)
            });
        default:
            return state;
    }
}