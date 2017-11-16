import { fetchDetails } from '../../actions/fetchDetails';
import * as types from '../../constants/actionTypes'
import configureMockStore from 'redux-mock-store'
import thunk from 'redux-thunk'
import fetchMock from 'fetch-mock'
import expect from 'expect'

const middlewares = [ thunk ]
const mockStore = configureMockStore(middlewares)

describe('fetchDetails async action', () => {

    afterEach(() => {
        fetchMock.reset()
        fetchMock.restore()
      })

    it('dispatches fetchingDetails on-call and receiveDetails when fetch is finished', () => {
        fetchMock
          .getOnce(`/Sentences/Details/123`, { 
              body: { references: [], entities: [] }})
          .catch(unmatchedUrl => {
              return fetchDetails(unmatchedUrl)
          })
          
        const expectedActions = [
          { type: types.FETCHING_DETAILS, tokenId: "123" },
          { 
            type: types.RECEIVE_DETAILS, 
            details: {
              '123': { references: [], entities: [] }
            }
          }
        ]
  
        const store = mockStore({ sentences: [] })   
        return store.dispatch(fetchDetails("123")).then(() => {
          // return of async actions
          expect(store.getActions()).toEqual(expectedActions)
        })
      })      
})