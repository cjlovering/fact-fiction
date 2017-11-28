import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import {
    Spinner,
    SpinnerSize
} from 'office-ui-fabric-react/lib/Spinner';
import { 
    VOTE_TRUE, 
    VOTE_FALSE, 
    VOTE_UNVOTED 
} from '../constants/voteTypes.js'
import _ from '../../stylesheets/components/_FactCard.scss'
import Button from './Button';
import VoteButtons from './VoteButtons';

const TOP_K_HITS = 5;
const MAX_SEN_LEN = 45;

export default class FactCard extends React.Component {
    static propTypes = {
        details: PropTypes.object.isRequired,
        fetchDetails: PropTypes.func.isRequired,
        content: PropTypes.string.isRequired,
        selectedEntryId: PropTypes.string.isRequired,
        id: PropTypes.string.isRequired,
        sentenceVote: PropTypes.string,
        selectEntry: PropTypes.func.isRequired,
        showingDetails: PropTypes.bool.isRequired,
        sentenceVote: PropTypes.string.isRequired,
        showDetails: PropTypes.func.isRequired,
        castVote: PropTypes.func.isRequired,
        voteTrue: PropTypes.number,
        voteFalse: PropTypes.number,
        fetchSimilarTokens: PropTypes.func.isRequired,
        similarTokenIds: PropTypes.object.isRequired
    }

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
            castVote,
            fetchSimilarTokens,
            similarTokenIds
        } = this.props;

        // Change CSS class if selected.
        const isSelected = id === selectedEntryId ? "card-selected" : "";

        // Render all the references.
        const hasDetails = details.hasOwnProperty('references') && details['references'];
        const { references, entities } = details;       
        
        const loadingDetails = hasDetails ? (
                references
                    .slice(0, TOP_K_HITS)
                    .map(ref => (
                        <tr key={shortid.generate()} style={{width: "100%"}}>
                            <th>
                                <a href={ref.link} > {this.cleanLink(ref.link)} </a>
                            </th>
                            {
                                ref.hasOwnProperty('bias') && ref.bias !== null ? (
                                    <th>
                                        <Bias {...ref.bias} />
                                    </th>
                                ) : null
                            }
                        </tr>
                    )
                )
            ) : (
                <tr>
                    <th>
                        <Spinner size={SpinnerSize.large} />
                    </th>
                </tr> 
            );

        const referencesJSX = (
            <div>
                <table style={{width: "100%"}}>
                    <tbody>
                        {loadingDetails}
                    </tbody>
                </table>
                <hr className="divider" />
            </div> 
        )                                 

        // Hide details if not showing (even if loaded.)
        const referencesJSXRendered = showingDetails 
            ? referencesJSX 
            : null;

        // Button for showing/hiding details 
        const expandButton = (
            <div style={{"padding": "10px", "textAlign": "center"}}>
                <Button
                    handleClick={() => this.handleButtonClick(hasDetails)}
                    text={!showingDetails ? "+" : "-"}
                />
            </div>
        );

        const selectAndFetchOnClick = () => {
            selectEntry(id);
            if (!similarTokenIds.hasOwnProperty(id)) {
                fetchSimilarTokens(id);
            }
        }

        return (
            <div
                className={`fact-card ${isSelected}`}
                onClick={selectAndFetchOnClick}
            >
                <div>
                    {content}
                </div>
                <hr className="divider" />
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

    cleanLink = (link) => {
        const remove = [ "http://", "https://" ];
        const step_1 = remove.reduce(
            (linc, pattern) => linc.replace(pattern, ""), link);
        return step_1.length <= MAX_SEN_LEN
            ? step_1
            : `${step_1.substring(0, MAX_SEN_LEN - 3)}...`;
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

const Bias = (props) => {
    return (
        <div>
            {props.biasType}
        </div>
    )
}