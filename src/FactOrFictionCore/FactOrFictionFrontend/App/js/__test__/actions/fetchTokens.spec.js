import * as actions from '../../actions/index'
import * as types from '../../constants/actionTypes'
import configureMockStore from 'redux-mock-store'
import thunk from 'redux-thunk'
import fetchMock from 'fetch-mock'
import expect from 'expect'
import { fetchTextEntry, fetchFeedTokens } from '../../actions/fetchTokens';

const middlewares = [ thunk ]
const mockStore = configureMockStore(middlewares)

describe('async actions', () => {
    afterEach(() => {
      fetchMock.reset()
      fetchMock.restore()
    })
  
    it('dispatches selectEntry and receiveTextEntryTokens when fetch text entry has been done', () => {
      fetchMock
        .getOnce(`/Sentences/Feed/?page=0`, { body: { sentences: [{ id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }]}})
        .catch(unmatchedUrl => {
            return fetchFeedTokens(unmatchedUrl)
        })
        
      const expectedActions = [
        { 
          type: types.RECEIVE_TOKENS, 
          tokens: {
            '123': { id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }
          }
        },
        {
          type: types.RECEIVE_FEED,
          feedTokenIds: ["123"]
        },
        { type: types.SELECT_ENTRY, id: "" }
      ]

      const store = mockStore({})   
      return store.dispatch(fetchFeedTokens()).then(() => {
        // return of async actions
        expect(store.getActions()).toEqual(expectedActions)
      });
    })

    it('dispatches selectEntry and receiveTextEntryTokens when fetch text entry has been done', () => {
      fetchMock
      .postOnce(`/TextEntries/Create/`, { body: { sentences: [{ id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }]}})
      .catch(unmatchedUrl => {
          return fetchTextEntry(unmatchedUrl)
      })
      
    const expectedActions = [
      { type: types.FETCHING_TOKENS, text: "This is a fact." },
      { 
        type: types.RECEIVE_TOKENS, 
        tokens: {
          '123': { id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }
        }
      },
      {
        type: types.RECEIVE_TEXT_ENTRY,
        textEntryTokenIds: ["123"]
      },
      { type: types.SELECT_ENTRY, id: "" }
    ]

    const store = mockStore({})   
    return store.dispatch(fetchTextEntry("This is a fact.")).then(() => {
      // return of async actions
      expect(store.getActions()).toEqual(expectedActions)
    });
  });
})