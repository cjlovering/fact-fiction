import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

import Button from './Button';
import Sentence from './Sentence';
import { VIEW_INPUT } from '../constants/viewTypes';

export default class ResultPane extends React.Component {
    render() {
        const { selectedEntryId, selectEntry, textEntryTokens, changeView } = this.props;

        return (
            <div>
                <div className="left-bar">
                    { textEntryTokens
                        .map(entry => (
                            <Sentence
                                {...entry}
                                selectedEntryId={selectedEntryId}
                                selectEntry={selectEntry}
                                key={shortid.generate()}
                                
                            />
                            )
                        )
                    }
                </div>
                <Button 
                    handleClick={() => changeView(VIEW_INPUT)} 
                    text="View Input"
                />
            </div>
        );
    }
}

ResultPane.propTypes = {
    selectedEntryId: PropTypes.string.isRequired,
    selectEntry: PropTypes.func.isRequired,
    textEntryTokens: PropTypes.array.isRequired,
    changeView: PropTypes.func.isRequired,
}