import * as actions from '../../actions/index'
import * as types from '../../constants/actionTypes'
import configureMockStore from 'redux-mock-store'
import thunk from 'redux-thunk'
import fetchMock from 'fetch-mock'
import expect from 'expect'
import { fetchTextEntry, fetchFeedSentences } from '../../actions/fetchSentences';

const middlewares = [ thunk ]
const mockStore = configureMockStore(middlewares)

describe('async actions', () => {
    afterEach(() => {
      fetchMock.reset()
      fetchMock.restore()
    })
  
    it('fetchFeedSentences dispatches selectEntry and receiveTextEntrySentences when fetch text entry has been done', () => {
      fetchMock
        .getOnce(`/Sentences/Feed/?page=0`, { body: { sentences: [{ id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }], votes: {}}})
        .catch(unmatchedUrl => {
            return fetchFeedSentences(unmatchedUrl)
        })
        
      const expectedActions = [
        { 
          type: types.RECEIVE_SENTENCES, 
          sentences: {
            '123': { id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }
          }
        },
        {
          type: types.RECEIVE_FEED,
          feedSentenceIds: ["123"]
        },
        { type: types.RECEIVE_VOTES, votes: {} }
      ]

      const store = mockStore({})   
      return store.dispatch(fetchFeedSentences()).then(() => {
        // return of async actions
        expect(store.getActions()).toEqual(expectedActions)
      });
    })

    it('fetchTextEntry dispatches selectEntry and receiveTextEntrySentences when fetch text entry has been done', () => {
      fetchMock
      .postOnce(`/TextEntries/Create/`, { body: { sentences: [{ id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }]}})
      .catch(unmatchedUrl => {
          return fetchTextEntry(unmatchedUrl)
      })
      
    const expectedActions = [
      { type: types.FETCHING_SENTENCES, text: "This is a fact." },
      { 
        type: types.RECEIVE_SENTENCES, 
        sentences: {
          '123': { id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }
        }
      },
      {
        type: types.RECEIVE_TEXT_ENTRY,
        textEntrySentenceIds: ["123"]
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