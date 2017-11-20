import { castVote } from '../../actions/castVote';
import * as types from '../../constants/actionTypes'
import { VOTE_TRUE, VOTE_FALSE } from '../../constants/voteTypes';
import configureMockStore from 'redux-mock-store'
import thunk from 'redux-thunk'
import fetchMock from 'fetch-mock'
import expect from 'expect'

const middlewares = [ thunk ]
const mockStore = configureMockStore(middlewares)

describe('castVote async action', () => {

    afterEach(() => {
        fetchMock.reset()
        fetchMock.restore()
      })

    it('dispatches castVote action and receive vote when action is finished', () => {
        fetchMock
          .postOnce(`/Votes/Cast/`, { 
                body: {
                    sentences: [
                        {
                            id: "123",
                            content: "This is a fact.",
                            type: "OBJECTIVE"
                        }
                    ],
                    votes: {
                        "123": {
                            sentenceId: "123",
                            type: "TRUE",
                            timestamp: ""
                        }
                    }
                }
            })
          .catch(unmatchedUrl => {
              return castVote(unmatchedUrl)
          })
          
        const expectedActions = [
            { 
                type: types.RECEIVE_TOKENS, 
                tokens: {
                  '123': { id: '123' , content: 'This is a fact.', type: 'OBJECTIVE' }
                }
            },
            { 
                type: types.RECEIVE_VOTES, 
                votes: {
                    "123": {
                        sentenceId: "123",
                        type: "TRUE",
                        timestamp: ""
                    }
                }
            }
        ]
  
        const store = mockStore({})   
        return store.dispatch(castVote({
            sentenceId: "123",
            type: "TRUE",
            timestamp: ""
        })).then(() => {
          // return of async actions
          expect(store.getActions()).toEqual(expectedActions);
        })
      })    
      
})