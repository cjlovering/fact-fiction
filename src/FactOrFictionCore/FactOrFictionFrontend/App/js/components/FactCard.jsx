import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import {VOTE_TRUE, VOTE_FALSE, VOTE_UNVOTED} from '../constants/voteTypes.js'
import _ from '../../stylesheets/components/_FactCard.scss'
import {
    Spinner,
    SpinnerSize
} from 'office-ui-fabric-react/lib/Spinner';

import Button from './Button';
import VoteButtons from './VoteButtons';

export default class FactCard extends React.Component {
    render() {
        const { 
            content, 
            selectedEntryId, 
            id,
            details, 
            showingDetails,
            sentenceVote,
            votes, 
            voteTrue, 
            voteFalse, 
            selectEntry, 
            castVote
        } = this.props;

        // Change CSS class if selected.
        const isSelected = id === selectedEntryId ? "card-selected" : "";

        // Render all the references.
        const hasDetails = 'references' in details && details['references'];
        const { references, entities } = details;        
        const referencesJSX = hasDetails ? (
            <div>
                <ul>
                {
                    references.map(ref => (
                        <li key={shortid.generate()}>
                            {ref.link}
                        </li>
                    ))
                }
                </ul>
            </div>                                  
        ) : (
            <div className="spinner">
                <Spinner size={SpinnerSize.large} />
            </div> 
        );

        // Hide details if not showing (even if loaded.)
        const referencesJSXRendered = showingDetails ? referencesJSX : null;

        // Button for showing/hiding details 
        const expandButton = (
            <div style={{"padding": "10px", "textAlign": "center"}}>
                <Button
                    handleClick={() => this.handleButtonClick(hasDetails)}
                    text={ !showingDetails ? "+" : "-" }
                />
            </div>
        );

        return (
            <div
                className={`fact-card ${isSelected}`}
                onClick={() => selectEntry(id)}
            >
                <div>
                    {content}
                </div>
                {referencesJSXRendered}
                {expandButton}
                <VoteButtons 
                    id={id}
                    sentenceVote={sentenceVote} 
                    voteTrue={voteTrue} 
                    voteFalse={voteFalse} 
                    castVote={castVote} 
                />
            </div>
        );
    }

    handleButtonClick = (hasDetails) => {
        const {
            id,
            fetchDetails, 
            showingDetails, 
            showDetails
        } = this.props;

        showDetails(id, !showingDetails);
        if (!hasDetails) { 
            fetchDetails(id); 
        }       
    }

}

FactCard.propTypes = {
    details: PropTypes.object.isRequired,
    fetchDetails: PropTypes.func.isRequired,
    content: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    sentenceVote: PropTypes.string,
    selectEntry: PropTypes.func.isRequired,
    showingDetails: PropTypes.bool.isRequired,
    sentenceVote: PropTypes.string.isRequired,
    showDetails: PropTypes.func.isRequired,
    castVote: PropTypes.func.isRequired,
    voteTrue: PropTypes.number,
    voteFalse: PropTypes.number
}