import textEntry from '../../reducers/textEntry'
import * as types from '../../constants/actionTypes'

describe('textEntry reducer', () => {

    const state = {
        isDoneFetchingFeed: false,
        isFetching: false,
        didInvalidate: false,
        tokens: {}, // map of token id to token
        textEntryTokenIds: [], // a list of text entry token ids 
        feedTokenIds: [] // a list of token ids for feed
    };

    it('should provide the initial state', () => {
        expect(textEntry(undefined, {})).toEqual({
            isFetching: false,
            didInvalidate: false,
            isDoneFetchingFeed: false,
            tokens: {}, // map of token id to token
            textEntryTokenIds: [], // a list of text entry token ids 
            feedTokenIds: [] // a list of token ids for feed
        })
    })

    it('should provide handle INVALIDATE_TEXT_ENTRY', () => {
        expect(textEntry(state, {
            type: types.INVALIDATE_TEXT_ENTRY
        })).toEqual({
            isFetching: false,
            didInvalidate: true,
            isDoneFetchingFeed: false,            
            tokens: {}, // map of token id to token
            textEntryTokenIds: [], // a list of text entry token ids 
            feedTokenIds: [] // a list of token ids for feed
        })
    })

    it('should provide handle FETCHING_TOKENS', () => {
        expect(textEntry(state, {
            type: types.FETCHING_TOKENS
        })).toEqual({
            isFetching: true,
            didInvalidate: false,
            isDoneFetchingFeed: false,            
            tokens: {}, // map of token id to token
            textEntryTokenIds: [], // a list of text entry token ids 
            feedTokenIds: [] // a list of token ids for feed
        })
    })

    it('should provide handle RECEIVE_TOKENS', () => {
        expect(textEntry({
            isFetching: true,
            didInvalidate: true,
            isDoneFetchingFeed: false,            
            tokens: {
                'abc': "a thing else",
                'ghi': "all good"
            }, // map of token id to token
            textEntryTokenIds: [], // a list of text entry token ids 
            feedTokenIds: [] // a list of token ids for feed
        }, {
            type: types.RECEIVE_TOKENS,
            tokens: {
                'abc': "something else",
                'def': "new thing"
            }
        })).toEqual({
            isFetching: false,
            isDoneFetchingFeed: false,
            didInvalidate: false,
            tokens: {
                'abc': "something else",
                'def': "new thing",
                'ghi': "all good"
            }, // map of token id to token
            textEntryTokenIds: [], // a list of text entry token ids 
            feedTokenIds: [] // a list of token ids for feed
        })
    })

    it('should provide handle RECEIVE_TEXT_ENTRY', () => {
        expect(textEntry(state, {
            type: types.RECEIVE_TEXT_ENTRY,
            textEntryTokenIds: ["abc"]
        })).toEqual({
            isFetching: false,
            isDoneFetchingFeed: false,
            didInvalidate: false,
            tokens: {}, // map of token id to token
            textEntryTokenIds: ["abc"], // a list of text entry token ids 
            feedTokenIds: [] // a list of token ids for feed
        })
    })

    it('should provide handle RECEIVE_TEXT_ENTRY - update', () => {
        expect(textEntry({...state,
            textEntryTokenIds: ["def"]
        }, {
            type: types.RECEIVE_TEXT_ENTRY,
            textEntryTokenIds: ["abc"]
        })).toEqual({
            isFetching: false,
            didInvalidate: false,
            isDoneFetchingFeed: false,            
            tokens: {}, // map of token id to token
            textEntryTokenIds: ["abc"], // a list of text entry token ids 
            feedTokenIds: [] // a list of token ids for feed
        })
    })

    it('should provide handle RECEIVE_FEED', () => {
        expect(textEntry(state, {
            type: types.RECEIVE_FEED,
            feedTokenIds: ["abc"]
        })).toEqual({
            isFetching: false,
            didInvalidate: false,
            isDoneFetchingFeed: false,            
            tokens: {}, // map of token id to token
            textEntryTokenIds: [], // a list of text entry token ids 
            feedTokenIds: ["abc"] // a list of token ids for feed
        })
    })

    it('should provide handle RECEIVE_FEED - merge', () => {
        expect(textEntry({...state,
            feedTokenIds: ["def"]
        }, {
            type: types.RECEIVE_FEED,
            feedTokenIds: ["abc"]
        })).toEqual({
            isDoneFetchingFeed: false,
            isFetching: false,
            didInvalidate: false,
            tokens: {}, // map of token id to token
            textEntryTokenIds: [], // a list of token ids for feed            
            feedTokenIds: ["def", "abc"] // a list of text entry token ids 
        })
    })
})