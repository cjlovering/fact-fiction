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
import References from './References';
import Entities from './Entities';

const TOP_K_HITS = 5;
const MAX_FACT_LEN = 175;
const MAX_LINK_LEN = 35;

export default class FactCard extends React.Component {
    static propTypes = {
        isInMiddlePane: PropTypes.bool.isRequired,
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
        fetchSimilarSentences: PropTypes.func.isRequired,
        similarSentenceIds: PropTypes.object.isRequired
    }
 
    render() {
        const { 
            isInMiddlePane,
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
            fetchSimilarSentences,
            similarSentenceIds
        } = this.props;

        // Change CSS class if selected.
        const isSelected = id === selectedEntryId ? "card-selected" : "";

        // Render all the references.
        const hasDetails = details.hasOwnProperty('references') && details['references'];
        const { references, entities } = details;

        // Entity elements
        const entityHeader = entities ? (
            <tr>
                <th>
                    Recognized entities
                </th>
            </tr>
        ) : null;
        const entitiesJSX = entities ? (
            <Entities
                entities={ 
                    entities
                        .slice(0, TOP_K_HITS)
                }
            />
        ) : null;


        const loadingDetails = hasDetails ? (
            <table style={{width: "100%"}}>
                <tbody>
                    <tr>
                        <th>
                            Related information
                        </th>
                        <th>
                            Site bias
                        </th>
                    </tr>
                    <References
                        references={ 
                            references
                                .slice(0, TOP_K_HITS)
                        }
                        cleanLink={text => this.cleanLink(text)}
                    />
                    {entityHeader}
                    {entitiesJSX}
                </tbody>
            </table>
        ) : (
            <div className="spinner-div">
                <Spinner size={SpinnerSize.large} />
            </div>
        );

        const referencesJSX = (
            <div>
                {loadingDetails}
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
                    content={ !showingDetails ? "+" : "-" }
                />
            </div>
        );

        const selectAndFetchOnClick = () => {
            if (isInMiddlePane) {
                selectEntry(id);
                if (!similarSentenceIds.hasOwnProperty(id)) {
                    fetchSimilarSentences(id);
                }
            } else{
                return;
            }
        }

        return (
            <div
                className={`fact-card ${isSelected}`}
                onClick={selectAndFetchOnClick}
            >
                <div>
                    { showingDetails ? content : this.cleanfact(content) }
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

    cleanfact = fact => {
        return this.shortenText(fact, MAX_FACT_LEN)
    }

    cleanLink = link => {
        const prefixes = [ "http://", "https://" ];
        const noPrefix = prefixes.reduce(
            (_link, pattern) => _link.replace(pattern, ""), link);
        return this.shortenText(noPrefix, MAX_LINK_LEN);
    }

    shortenText = (text, length) => text.length < length 
        ? text 
        : `${text.substring(0, length - 3)}...`;

    handleButtonClick = hasDetails => {
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
