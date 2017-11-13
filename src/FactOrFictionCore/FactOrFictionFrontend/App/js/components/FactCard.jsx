import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_FactCard.scss'

export default class FactCard extends React.Component {
    render() {
        const { sentence, selectedEntryId, id, selectEntry } = this.props;
        const isSelected = id === selectedEntryId ? "selected" : "";
        return (
            <div
                className={`fact-card ${isSelected}`}
                onClick={() => selectEntry(id)}
            >
                <div>
                    {sentence}
                </div>
                <div className="vote-buttons" />
            </div>
        );
    }
}

FactCard.propTypes = {
    sentence: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired
}