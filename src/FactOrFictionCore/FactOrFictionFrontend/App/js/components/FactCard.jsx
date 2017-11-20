import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import _ from '../../stylesheets/components/_FactCard.scss'
import {
    Spinner,
    SpinnerSize
} from 'office-ui-fabric-react/lib/Spinner';

import Button from './Button';

export default class FactCard extends React.Component {

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

    render() {
        const { 
            content, 
            selectedEntryId, 
            id, 
            selectEntry, 
            details, 
            showingDetails
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
        const button = (
            <Button
                handleClick={() => this.handleButtonClick(hasDetails)}
                text={ !showingDetails ? "+" : "-" }
            />
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
                {button}
                <div className="vote-buttons" />
            </div>
        );
    }
}

FactCard.propTypes = {
    details: PropTypes.object.isRequired,
    fetchDetails: PropTypes.func.isRequired,
    content: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired,
    showingDetails: PropTypes.bool.isRequired,
    showDetails: PropTypes.func.isRequired
}