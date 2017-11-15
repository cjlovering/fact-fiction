import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_FactCard.scss'

export default class FactCard extends React.Component {
    render() {
        const { content, selectedEntryId, id, selectEntry } = this.props;
        const isSelected = id === selectedEntryId ? "card-selected" : "";
        return (
            <div
                className={`fact-card ${isSelected}`}
                onClick={() => selectEntry(id)}
            >
                <div>
                    {content}
                </div>
                <div className="vote-buttons" />
            </div>
        );
    }
}

FactCard.propTypes = {
    content: PropTypes.string.isRequired,
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired
}