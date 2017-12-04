import sentenceDetails from '../../reducers/sentenceDetails'
import * as types from '../../constants/actionTypes'

describe('sentenceDetails reducer', () => {

    const state = {
        details: {
            "abc": {
                entities: [],
                references: []
            }
        },
        isFetching: false
    }

    it('should provide the initial state', () => {
        expect(sentenceDetails(undefined, {})).toEqual({
            details: {},
            isFetching: false
        })
    })

    it('should handle FETCHING_DETAILS action', () => {
        expect(sentenceDetails(state, { type: types.FETCHING_DETAILS, sentenceId: "123" }))
            .toEqual({
                details: {
                    "abc": {
                        entities: [],
                        references: []
                    }
                },
                isFetching: true
            }
        );
    })

    it('should handle RECEIVE_DETAILS action - isFetching', () => {
        const isFetching = true;
        expect(sentenceDetails(
                { ...state, isFetching}, 
            {
                type: types.RECEIVE_DETAILS,
                details: { "abc": {
                entities: [],
                references: []
            }}}))
            .toEqual({
                details: {
                    "abc": {
                        entities: [],
                        references: []
                    }
                },
                isFetching: false
            }
        );
    })

    it('should handle RECEIVE_DETAILS action - isFetching', () => {
        expect(sentenceDetails(state, 
            {
                type: types.RECEIVE_DETAILS,
                details: { "456": {
                entities: [],
                references: []
            }}}))
            .toEqual({
                details: {
                    "abc": {
                        entities: [],
                        references: []
                    },
                    "456": {
                        entities: [],
                        references: []
                    }
                },
                isFetching: false
            }
        );
    })

    it('should handle RECEIVE_DETAILS action - updating', () => {
        expect(sentenceDetails(state, 
            {
                type: types.RECEIVE_DETAILS,
                details: { "abc": {
                entities: ["BLAH"],
                references: []
            }}}))
            .toEqual({
                details: {
                    "abc": {
                        entities: ["BLAH"],
                        references: []
                    }
                },
                isFetching: false
            }
        );
    })

    it('should ignore unknown actions', () => {
        expect(sentenceDetails(state, { type: 'unknown' })).toBe(state)
    })
})
