import detailsShown from '../../reducers/detailsShown'
import * as types from '../../constants/actionTypes'
import { SHOW_DETAILS } from '../../constants/actionTypes';

describe('detailsShown reducer', () => {
    const state = {
        "abc": true,
        "def": false
    };
    it('should provide the initial state', () => {
        expect(detailsShown(undefined, {})).toEqual({})
    })
    it('should handle undefined type', () => {
        expect(detailsShown(state, {
            type: "undefined"
        })).toEqual(state)
    })
    it('should handle SHOW_DETAILS - new', () => {
        expect(detailsShown(state, {
            type: SHOW_DETAILS,
            id: "ghi",
            show: true
        })).toEqual({
            "abc": true,
            "def": false,
            "ghi": true
        })
    })
    it('should handle SHOW_DETAILS - update', () => {
        expect(detailsShown(state, {
            type: SHOW_DETAILS,
            id: "def",
            show: true
        })).toEqual({
            "abc": true,
            "def": true
        })
    })

})