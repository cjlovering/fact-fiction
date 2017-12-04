import sentences from '../../reducers/sentences'
import * as types from '../../constants/actionTypes'

describe('sentences reducer', () => {

    const state = {
        isDoneFetchingFeed: false,
        isFetching: false,
        didInvalidate: false,
        sentences: {}, // map of sentence id to sentence
        textEntrySentenceIds: [], // a list of text entry sentence ids 
        feedSentenceIds: [], // a list of sentence ids for feed
        votes: {}
    };

    it('should provide the initial state', () => {
        expect(sentences(undefined, {})).toEqual({
            isFetching: false,
            didInvalidate: false,
            isDoneFetchingFeed: false,
            sentences: {}, // map of sentence id to sentence
            textEntrySentenceIds: [], // a list of text entry sentence ids 
            feedSentenceIds: [], // a list of sentence ids for feed
            votes: {}
        })
    })

    it('should provide handle INVALIDATE_TEXT_ENTRY', () => {
        expect(sentences(state, {
            type: types.INVALIDATE_TEXT_ENTRY
        })).toEqual({
            isFetching: false,
            didInvalidate: true,
            isDoneFetchingFeed: false,            
            sentences: {}, // map of sentence id to sentence
            textEntrySentenceIds: [], // a list of text entry sentence ids 
            feedSentenceIds: [], // a list of sentence ids for feed
            votes: {}
        })
    })

    it('should provide handle FETCHING_SENTENCES', () => {
        expect(sentences(state, {
            type: types.FETCHING_SENTENCES
        })).toEqual({
            isFetching: true,
            didInvalidate: false,
            isDoneFetchingFeed: false,            
            sentences: {}, // map of sentence id to sentence
            textEntrySentenceIds: [], // a list of text entry sentence ids 
            feedSentenceIds: [], // a list of sentence ids for feed
            votes: {}
        })
    })

    it('should provide handle RECEIVE_SENTENCES', () => {
        expect(sentences({
            isFetching: true,
            didInvalidate: true,
            isDoneFetchingFeed: false,            
            sentences: {
                'abc': "a thing else",
                'ghi': "all good"
            }, // map of sentence id to sentence
            textEntrySentenceIds: [], // a list of text entry sentence ids 
            feedSentenceIds: [], // a list of sentence ids for feed
            votes: {}
        }, {
            type: types.RECEIVE_SENTENCES,
            sentences: {
                'abc': "something else",
                'def': "new thing"
            }
        })).toEqual({
            isFetching: false,
            isDoneFetchingFeed: false,
            didInvalidate: false,
            sentences: {
                'abc': "something else",
                'def': "new thing",
                'ghi': "all good"
            }, // map of sentence id to sentence
            textEntrySentenceIds: [], // a list of text entry sentence ids 
            feedSentenceIds: [], // a list of sentence ids for feed
            votes: {}
        })
    })

    it('should provide handle RECEIVE_TEXT_ENTRY', () => {
        expect(sentences(state, {
            type: types.RECEIVE_TEXT_ENTRY,
            textEntrySentenceIds: ["abc"]
        })).toEqual({
            isFetching: false,
            isDoneFetchingFeed: false,
            didInvalidate: false,
            sentences: {}, // map of sentence id to sentence
            textEntrySentenceIds: ["abc"], // a list of text entry sentence ids 
            feedSentenceIds: [], // a list of sentence ids for feed
            votes: {}
        })
    })

    it('should provide handle RECEIVE_TEXT_ENTRY - update', () => {
        expect(sentences({...state,
            textEntrySentenceIds: ["def"]
        }, {
            type: types.RECEIVE_TEXT_ENTRY,
            textEntrySentenceIds: ["abc"]
        })).toEqual({
            isFetching: false,
            didInvalidate: false,
            isDoneFetchingFeed: false,            
            sentences: {}, // map of sentence id to sentence
            textEntrySentenceIds: ["abc"], // a list of text entry sentence ids 
            feedSentenceIds: [], // a list of sentence ids for feed
            votes: {}
        })
    })

    it('should provide handle RECEIVE_FEED', () => {
        expect(sentences(state, {
            type: types.RECEIVE_FEED,
            feedSentenceIds: ["abc"]
        })).toEqual({
            isFetching: false,
            didInvalidate: false,
            isDoneFetchingFeed: false,            
            sentences: {}, // map of sentence id to sentence
            textEntrySentenceIds: [], // a list of text entry sentence ids 
            feedSentenceIds: ["abc"], // a list of sentence ids for feed
            votes: {}
        })
    })

    it('should provide handle RECEIVE_FEED - merge', () => {
        expect(sentences({...state,
            feedSentenceIds: ["def"]
        }, {
            type: types.RECEIVE_FEED,
            feedSentenceIds: ["abc"]
        })).toEqual({
            isDoneFetchingFeed: false,
            isFetching: false,
            didInvalidate: false,
            sentences: {}, // map of sentence id to sentence
            textEntrySentenceIds: [], // a list of sentence ids for feed            
            feedSentenceIds: ["def", "abc"], // a list of text entry sentence ids 
            votes: {}
        })
    })
})