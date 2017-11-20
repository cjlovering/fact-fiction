import React from 'react';
import PropTypes from 'prop-types';
import { VOTE_TRUE, VOTE_FALSE, VOTE_UNVOTED } from '../constants/voteTypes.js'
import _ from '../../stylesheets/components/_VoteButtons.scss'

export default class VoteButtons extends React.Component {
    render() {
        const { 
            sentenceVote, 
            voteTrue, 
            voteFalse 
        } = this.props;

        const trueClass = `vote-button-true${sentenceVote == VOTE_TRUE ? `-pressed` : ""}`
        const falseClass = `vote-button-false${sentenceVote == VOTE_FALSE ? `-pressed` : ""}`

        return (
            <div className="vote-buttons">
                <table style={{"width": "100%"}}>
                    <tbody>
                        <tr>
                            <th style={{"textAlign": "right"}}>
                                <button 
                                    className={"ff-Button change-view-button ms-Button"}
                                    onClick={() => this.handleClick(VOTE_TRUE)}>
                                    <i
                                        className={`ms-Icon ms-Icon--triangleUp ${trueClass}`}
                                        aria-hidden="true"
                                    />
                                    <span>  True {voteTrue}</span>
                                </button>
                            </th>
                            <th style={{"textAlign": "left"}}>
                                <button 
                                    className={"ff-Button change-view-button ms-Button"}
                                    onClick={() => this.handleClick(VOTE_FALSE)}>
                                    <i
                                        className={`ms-Icon ms-Icon--triangleDown ${falseClass}`}
                                        aria-hidden="true"
                                    />
                                    <span>  False {voteFalse}</span>
                                </button>
                            </th> 
                        </tr>
                    </tbody>
                </table>
            </div>
        );
    }

    handleClick(type) {
        const { id, castVote } = this.props;
        castVote(id, type);
    }
}
